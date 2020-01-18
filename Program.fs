open System

type Buffer = string list

type Cursor = { row: int; col: int}

type Action = CursorMove | CharInserted | CharDeleted

let render (buffer:Buffer, cursor: Cursor) = 
    Console.Clear ()
    Console.SetCursorPosition (0, 0)
    for str in buffer do
        Console.WriteLine (str)
    Console.CursorTop <- cursor.row
    Console.CursorLeft <- cursor.col

let handleInput (buffer:Buffer, cursor: Cursor) = 
    let a = Console.ReadKey (true)
    match a.Key with
    | ConsoleKey.UpArrow -> (buffer, { cursor with row = cursor.row - 1 }, CursorMove)
    | ConsoleKey.DownArrow -> (buffer, { cursor with row = cursor.row + 1 }, CursorMove)
    | ConsoleKey.LeftArrow -> (buffer, { cursor with col = cursor.col - 1 }, CursorMove)
    | ConsoleKey.RightArrow -> (buffer, { cursor with col = cursor.col + 1 }, CursorMove)
    | ConsoleKey.Backspace -> (List.take (cursor.row) buffer @ 
                               [buffer.[cursor.row].Remove(cursor.col, 1)] @
                               List.skip (cursor.row + 1) buffer, { cursor with col = cursor.col - 1 }, CharDeleted)
    | _ -> (List.take (cursor.row) buffer @ 
            [buffer.[cursor.row].Insert(cursor.col, a.KeyChar.ToString())] @
            List.skip (cursor.row + 1) buffer, { cursor with col = cursor.col + 1 }, CharInserted)

[<EntryPoint>]
let main argv =
    let mutable buffer = ["first"; "second"; "second"; "second"; "second"]
    let mutable cursor = { row = 1; col = 2 }

    render (buffer, cursor)
    while true do
        let (newBuffer, newCursor, action) = handleInput(buffer, cursor)
        buffer <- newBuffer
        cursor <- newCursor

        match action with
        | CursorMove -> Console.SetCursorPosition (newCursor.col, newCursor.row)
        | CharInserted | CharDeleted -> 
            Console.SetCursorPosition (0, newCursor.row)
            Console.Write(new string(' ', Console.BufferWidth)); 
            Console.SetCursorPosition (0, newCursor.row)
            Console.Write (newBuffer.[newCursor.row])
            Console.SetCursorPosition (newCursor.col, newCursor.row)
    0 // return an integer exit code