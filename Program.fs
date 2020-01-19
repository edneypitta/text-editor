open System
open Editor

type Action = CursorMove | CharInserted | CharDeleted

let render (buffer: Buffer, cursor: Cursor) = 
    Console.Clear ()
    Console.SetCursorPosition (0, 0)
    for str in buffer do
        Console.WriteLine (str)
    Console.CursorTop <- cursor.Row
    Console.CursorLeft <- cursor.Col

let handleInput (buffer:Buffer, cursor: Cursor) = 
    let a = Console.ReadKey (true)
    match a.Key with
    | ConsoleKey.UpArrow -> (buffer, { cursor with Row = cursor.Row - 1 }, CursorMove)
    | ConsoleKey.DownArrow -> (buffer, { cursor with Row = cursor.Row + 1 }, CursorMove)
    | ConsoleKey.LeftArrow -> (buffer, { cursor with Col = cursor.Col - 1 }, CursorMove)
    | ConsoleKey.RightArrow -> (buffer, { cursor with Col = cursor.Col + 1 }, CursorMove)
    | ConsoleKey.Backspace -> (List.take (cursor.Row) buffer @ 
                               [buffer.[cursor.Row].Remove(cursor.Col, 1)] @
                               List.skip (cursor.Row + 1) buffer, { cursor with Col = cursor.Col - 1 }, CharDeleted)
    | _ -> (List.take (cursor.Row) buffer @ 
            [buffer.[cursor.Row].Insert(cursor.Col, a.KeyChar.ToString())] @
            List.skip (cursor.Row + 1) buffer, { cursor with Col = cursor.Col + 1 }, CharInserted)

[<EntryPoint>]
let main argv =
    let mutable buffer = ["first"; "second"; "second"; "second"; "second"]
    let mutable cursor = { Row = 1; Col = 2 }

    render (buffer, cursor)
    while true do
        let (newBuffer, newCursor, action) = handleInput(buffer, cursor)
        buffer <- newBuffer
        cursor <- newCursor

        match action with
        | CursorMove -> Console.SetCursorPosition (newCursor.Col, newCursor.Row)
        | CharInserted | CharDeleted -> 
            Console.SetCursorPosition (0, newCursor.Row)
            Console.Write(new string(' ', Console.BufferWidth)); 
            Console.SetCursorPosition (0, newCursor.Row)
            Console.Write (newBuffer.[newCursor.Row])
            Console.SetCursorPosition (newCursor.Col, newCursor.Row)
    0 // return an integer exit code