open System
open Editor

type Action = CursorMove | CharInserted | CharDeleted | Enter

let render editor = 
  Console.Clear ()
  Console.SetCursorPosition (0, 0)
  for str in editor.Buffer do
      Console.WriteLine (str)
  Console.SetCursorPosition (editor.Cursor.Col, editor.Cursor.Row)

let handleInput editor = 
  let keyInfo = Console.ReadKey (true)
  let char = keyInfo.KeyChar.ToString()

  match keyInfo.Key with
  | ConsoleKey.UpArrow -> ({ editor with Cursor = up editor.Cursor }, CursorMove)
  | ConsoleKey.DownArrow -> ({ editor with Cursor = down editor.Cursor }, CursorMove)
  | ConsoleKey.LeftArrow -> ({ editor with Cursor = left editor.Cursor }, CursorMove)
  | ConsoleKey.RightArrow -> ({ editor with Cursor = right editor.Cursor }, CursorMove)
  | ConsoleKey.Backspace -> (removeChar editor, CharDeleted)
  | ConsoleKey.Enter -> (enter editor, Enter)
  | _ -> (insertChar editor char, CharInserted)

let writeRow (lines: Buffer) row =
  Console.SetCursorPosition (0, row)
  Console.Write (new string(' ', Console.BufferWidth)); 
  Console.SetCursorPosition (0, row)
  Console.Write (lines.[row])

[<EntryPoint>]
let main _ =
  let mutable editor = {
    Buffer = ["the quick brown fox"; "jumped over the lazy dog"]
    Cursor = { Row = 0; Col = 7 }
  }

  render editor

  while true do
    let (newEditor, action) = handleInput editor
    editor <- newEditor

    match action with
    | CharInserted | CharDeleted -> 
        writeRow editor.Buffer editor.Cursor.Row
    | Enter ->
        let previousRow = editor.Cursor.Row - 1
        let lastRow = editor.Buffer.Length - 1
        for row in [previousRow..lastRow] do
            writeRow editor.Buffer row
    | _ -> ()

    Console.SetCursorPosition (editor.Cursor.Col, editor.Cursor.Row)
  0