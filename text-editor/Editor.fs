module Editor

  type Buffer = string list
  type Cursor = { Row: int; Col: int }

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

  let up cursor = { cursor with Row = cursor.Row - 1 }
  let down cursor = { cursor with Row = cursor.Row + 1 }
  let left cursor = { cursor with Col = cursor.Col - 1 }
  let right cursor = { cursor with Col = cursor.Col + 1 }
  let leftMost cursor = { cursor with Col = 0 }

  let removeChar editor = 
    {
      Cursor = editor.Cursor |> down |> leftMost
      Buffer = apply editor (fun line -> line.Remove(editor.Cursor.Col, 1))
    }
  
  let insertChar editor char =
    {
      Cursor = editor.Cursor |> right
      Buffer = apply editor (fun line -> line.Insert(editor.Cursor.Col, char)) 
    }

  let enter editor =
    {
      Cursor = editor.Cursor |> down |> leftMost
      Buffer = splitLine editor
    }

