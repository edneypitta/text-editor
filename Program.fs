open System
open Editor

type Action = CursorMove | CharInserted | CharDeleted | Enter

let render (Buffer lines) cursor = 
    Console.Clear ()
    Console.SetCursorPosition (0, 0)
    for str in lines do
        Console.WriteLine (str)
    Console.SetCursorPosition (cursor.Col, cursor.Row)

let handleInput buffer cursor = 
    let keyInfo = Console.ReadKey (true)
    let char = keyInfo.KeyChar.ToString()

    match keyInfo.Key with
    | ConsoleKey.UpArrow -> (buffer, up cursor, CursorMove)
    | ConsoleKey.DownArrow -> (buffer, down cursor, CursorMove)
    | ConsoleKey.LeftArrow -> (buffer, left cursor, CursorMove)
    | ConsoleKey.RightArrow -> (buffer, right cursor, CursorMove)
    | ConsoleKey.Backspace -> (removeChar buffer cursor, left cursor, CharDeleted)
    | ConsoleKey.Enter -> (splitLine buffer cursor, cursor |> down |> leftMost, Enter)
    | _ -> (insertChar buffer cursor char, right cursor, CharInserted)

let writeRow (Buffer lines) row =
    Console.SetCursorPosition (0, row)
    Console.Write(new string(' ', Console.BufferWidth)); 
    Console.SetCursorPosition (0, row)
    Console.Write (lines.[row])

[<EntryPoint>]
let main _ =
    let mutable buffer = Buffer ["first"; "second"; "third"; "fourth"]
    let mutable cursor = { Row = 1; Col = 2 }

    render buffer cursor

    while true do
        let (newBuffer, newCursor, action) = handleInput buffer cursor
        buffer <- newBuffer
        cursor <- newCursor

        match action with
        | CharInserted | CharDeleted -> 
            writeRow buffer cursor.Row
        | Enter -> 
            let (Buffer lines) = buffer
            for row in [(cursor.Row - 1)..lines.Length - 1] do
                writeRow buffer row
        | _ -> ()

        Console.SetCursorPosition (cursor.Col, cursor.Row)
    0