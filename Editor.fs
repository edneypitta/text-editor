module Editor

    type Buffer = string list
    type Cursor = { Row: int; Col: int} 

    let removeChar (buffer: Buffer, cursor) = 
        List.take (cursor.Row) buffer @ 
        [buffer.[cursor.Row].Remove(cursor.Col, 1)] @
        List.skip (cursor.Row + 1) buffer

    let insertChar (buffer: Buffer, cursor, char) =
        List.take (cursor.Row) buffer @ 
        [buffer.[cursor.Row].Insert(cursor.Col, char)] @
        List.skip (cursor.Row + 1) buffer

    let up (cursor) = { cursor with Row = cursor.Row - 1 }
    let down (cursor) = { cursor with Row = cursor.Row + 1 }
    let left (cursor) = { cursor with Col = cursor.Col - 1 }
    let right (cursor) = { cursor with Col = cursor.Col + 1 }

