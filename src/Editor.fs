module Editor
  open Util

  type Buffer = string list
  type Cursor = { Row: int; Col: int; LastAttemptedCol: int }

  type Editor = {
    Cursor: Cursor
    Buffer: Buffer
  }

  let private apply editor func =
    editor.Buffer |>
    List.indexed |>
    List.map (fun (index, line) ->
      match index with
      | i when i = editor.Cursor.Row -> func line
      | _ -> line)
      
  let private splitLine editor =
    editor.Buffer |>
    List.indexed |>
    List.fold (fun acc elem -> 
               acc @
               match elem with
               | (index, line) when index = editor.Cursor.Row -> 
                 [
                   line.Substring(0, editor.Cursor.Col)
                   line.Substring(editor.Cursor.Col, line.Length - line.Substring(0, editor.Cursor.Col).Length)
                 ]
               | (_, line) -> [line]) []

  let private moveRow editor newRow =
    let lastAttempedCol = editor.Cursor.LastAttemptedCol
    let newRowLength = editor.Buffer.[newRow].Length
    let newCursor = 
      match editor.Cursor.Col with
      | _ when lastAttempedCol > newRowLength ->
        { Row = newRow; Col = newRowLength; LastAttemptedCol = lastAttempedCol }
      | col when col > newRowLength ->
        { Row = newRow; Col = newRowLength; LastAttemptedCol = col }
      | _ ->
        { Row = newRow; Col = lastAttempedCol; LastAttemptedCol = lastAttempedCol }
    { editor with Cursor = newCursor }         

  let up editor =
    match editor.Cursor.Row with
    | 0 -> editor
    | row -> moveRow editor (dec row)

  let down editor =
    match editor.Cursor.Row with
    | row when row = dec editor.Buffer.Length -> editor
    | row -> moveRow editor (inc row)

  let left editor =
    let col =  max 0 (dec editor.Cursor.Col)
    { editor with Cursor = { editor.Cursor with Col = col; LastAttemptedCol = col } }

  let right editor = 
    let row = editor.Buffer.[editor.Cursor.Row]
    let col = min row.Length (inc editor.Cursor.Col)
    { editor with Cursor = { editor.Cursor with Col = col; LastAttemptedCol = col } }
    
  let leftMost editor =
    { editor with Cursor = { editor.Cursor with Col = 0 } }

  let removeChar editor =
    let row = editor.Buffer.[editor.Cursor.Row]
    match editor.Cursor.Col with
    | 0 -> editor
    | col when col = row.Length -> left editor
    | col -> 
      let newCol = dec col
      { editor with
          Cursor = { editor.Cursor with Col = newCol; LastAttemptedCol = newCol }
          Buffer = apply editor (fun line -> line.Remove(col, 1))
      }
  
  let insertChar editor char =
    let col = editor.Cursor.Col
    let newCol = inc col
    { editor with
        Cursor = { editor.Cursor with Col = newCol; LastAttemptedCol = newCol }
        Buffer = apply editor (fun line -> line.Insert(col, char)) 
    }

  let enter editor =
    { editor with
        Cursor = { Col = 0; Row = inc editor.Cursor.Row; LastAttemptedCol = 0 }
        Buffer = splitLine editor
    }

