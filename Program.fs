open System
open Editor

type Action = CursorMove | CharInserted | CharDeleted

let render (buffer: Buffer) cursor = 
    let (Buffer lines) = buffer
    Console.Clear ()
    Console.SetCursorPosition (0, 0)
    for str in lines do
        Console.WriteLine (str)
    Console.SetCursorPosition (cursor.Col, cursor.Row)

let handleInput (buffer: Buffer) cursor = 
    let keyInfo = Console.ReadKey (true)
    let char = keyInfo.KeyChar.ToString()

    match keyInfo.Key with
    | ConsoleKey.UpArrow -> (buffer, up cursor, CursorMove)
    | ConsoleKey.DownArrow -> (buffer, down cursor, CursorMove)
    | ConsoleKey.LeftArrow -> (buffer, left cursor, CursorMove)
    | ConsoleKey.RightArrow -> (buffer, right cursor, CursorMove)
    | ConsoleKey.Backspace -> (removeChar buffer cursor, left cursor, CharDeleted)
    | _ -> (insertChar buffer cursor char, right cursor, CharInserted)

[<EntryPoint>]
let main _ =
    let mutable buffer = Buffer ["first"; "second"; "second"; "second"; "second"]
    let mutable cursor = { Row = 1; Col = 2 }

    render buffer cursor

    while true do
        let (newBuffer, newCursor, action) = handleInput buffer cursor
        buffer <- newBuffer
        cursor <- newCursor

        match action with
        | CursorMove -> Console.SetCursorPosition (newCursor.Col, newCursor.Row)
        | CharInserted | CharDeleted -> 
            Console.SetCursorPosition (0, newCursor.Row)
            Console.Write(new string(' ', Console.BufferWidth)); 
            Console.SetCursorPosition (0, newCursor.Row)
            Console.Write (match newBuffer with Buffer b -> b.[newCursor.Row])
            Console.SetCursorPosition (newCursor.Col, newCursor.Row)
    0