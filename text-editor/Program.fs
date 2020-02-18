open System
open Editor
open Util

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
  | ConsoleKey.UpArrow -> (up editor, CursorMove)
  | ConsoleKey.DownArrow -> (down editor, CursorMove)
  | ConsoleKey.LeftArrow -> (left editor, CursorMove)
  | ConsoleKey.RightArrow -> (right editor, CursorMove)
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
    Buffer = ["the quick"; "brown"; "fox jumped over the lazy dog"]
    Cursor = { Row = 2; Col = 15; LastAttemptedCol = 15 }
  }

  render editor

  while true do
    let (newEditor, action) = handleInput editor
    editor <- newEditor

    match action with
    | CharInserted | CharDeleted -> 
      writeRow editor.Buffer editor.Cursor.Row
    | Enter ->
      let previousRow = dec editor.Cursor.Row
      let lastRow = dec editor.Buffer.Length
      for row in [previousRow..lastRow] do
        writeRow editor.Buffer row
    | _ -> ()

    Console.SetCursorPosition (editor.Cursor.Col, editor.Cursor.Row)
  0