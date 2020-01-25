open System
open EditorPT

type Action = CursorMove | CharInserted | CharDeleted | Enter

let render { PieceTable = pieceTable; Cursor = cursor } =
    Console.Clear ()
    Console.SetCursorPosition (0, 0)
    for piece in pieceTable.Pieces do
        match piece.Source with
        | Original -> Console.Write(pieceTable.Original.Substring(piece.Start, piece.Length))
        | Add -> Console.Write(pieceTable.Add.Substring(piece.Start, piece.Length))
    Console.SetCursorPosition (cursor.Col, cursor.Row)

let handleInput editor = 
    let keyInfo = Console.ReadKey (true)
    let char = keyInfo.KeyChar.ToString()

    match keyInfo.Key with
    //| ConsoleKey.UpArrow -> (buffer, up cursor, CursorMove)
    //| ConsoleKey.DownArrow -> (buffer, down cursor, CursorMove)
    | ConsoleKey.LeftArrow -> (left editor, CursorMove)
    | ConsoleKey.RightArrow -> (right editor, CursorMove)
    //| ConsoleKey.Backspace -> (removeChar buffer cursor, left cursor, CharDeleted)
    //| ConsoleKey.Enter -> (splitLine buffer cursor, cursor |> down |> leftMost, Enter)
    | _ -> (insertChar editor char, CharInserted)

    //let writeRow (Buffer lines) row =
    //    Console.SetCursorPosition (0, row)
    //    Console.Write(new string(' ', Console.BufferWidth)); 
    //    Console.SetCursorPosition (0, row)
    //    Console.Write (lines.[row])

[<EntryPoint>]
let main _ =
    let mutable editor = {
        Cursor = { Row = 0; Col = 0; Offset = 0 }
        PieceTable = { 
            Original = "the quick brown fox\njumped over the lazy dog"
            Add = ""
            Pieces = [{ Start = 0; Length = 44; Source = Original }]
        }
    }

    render editor

    while true do
        let (newEditor, action) = handleInput editor
        editor <- newEditor

        match action with
        //| CharInserted | CharDeleted -> 
        //    writeRow buffer cursor.Row
        | CharInserted | CharDeleted -> 
            render editor
        //| Enter -> 
        //    let (Buffer lines) = buffer
        //    for row in [(cursor.Row - 1)..lines.Length - 1] do
        //        writeRow buffer row
        | CursorMove -> Console.SetCursorPosition (editor.Cursor.Col, editor.Cursor.Row)
        | _ -> ()
    0