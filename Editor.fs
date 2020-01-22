module Editor

    type Buffer = Buffer of string list
    type Cursor = { Row: int; Col: int } 

    let private apply buffer lineIndex func =
        let (Buffer lines) = buffer

        lines |>
        List.indexed |>
        List.map (fun (index, line) ->
            match index with
            | i when i = lineIndex -> func line
            | _ -> line) |>
        Buffer

    let removeChar buffer cursor = 
        apply buffer cursor.Row (fun line -> line.Remove(cursor.Col, 1))

    let insertChar buffer cursor char =
        apply buffer cursor.Row (fun line -> line.Insert(cursor.Col, char)) 

    let up cursor = { cursor with Row = cursor.Row - 1 }
    let down cursor = { cursor with Row = cursor.Row + 1 }
    let left cursor = { cursor with Col = cursor.Col - 1 }
    let right cursor = { cursor with Col = cursor.Col + 1 }

