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

let handleInput (buffer, cursor) = 
    let a = Console.ReadKey (true)
    match a.Key with
    | ConsoleKey.UpArrow -> (buffer, up cursor, CursorMove)
    | ConsoleKey.DownArrow -> (buffer, down cursor, CursorMove)
    | ConsoleKey.LeftArrow -> (buffer, left cursor, CursorMove)
    | ConsoleKey.RightArrow -> (buffer, right cursor, CursorMove)
    | ConsoleKey.Backspace -> (removeChar (buffer, cursor), left cursor, CharDeleted)
    | _ -> (insertChar (buffer, cursor, a.KeyChar.ToString()), right cursor, CharInserted)

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