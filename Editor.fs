module Editor

    type Buffer = string list
    type Cursor = { Row: int; Col: int } 

    let private apply buffer lineIndex func =
        buffer |>
        List.indexed |>
        List.map (fun (index, line) ->
            match index with
            | i when i = lineIndex -> func line
            | _ -> line)

    let removeChar (buffer: Buffer) cursor = 
        apply buffer cursor.Row (fun line -> line.Remove(cursor.Col, 1))

    let insertChar (buffer: Buffer) cursor char =
        apply buffer cursor.Row (fun line -> line.Insert(cursor.Col, char))

    let up cursor = { cursor with Row = cursor.Row - 1 }
    let down cursor = { cursor with Row = cursor.Row + 1 }
    let left cursor = { cursor with Col = cursor.Col - 1 }
    let right cursor = { cursor with Col = cursor.Col + 1 }

