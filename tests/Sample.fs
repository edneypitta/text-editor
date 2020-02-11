module Tests

open Expecto
open Editor

[<Tests>]
let tests =
  testList "up" [
    testCase "top row" <| fun _ ->
      let editor = {
        Buffer = ["the quick brown fox"]
        Cursor = { Row = 0; Col = 7; LastAttemptedCol = 7 }
      }
      let { Cursor = cursor } = up editor

      Expect.equal cursor.Row 0 "Should stay in top row"
      Expect.equal cursor.Col 7 "Should stay in current col"
      Expect.equal cursor.LastAttemptedCol 7 "Should store last attempted row"

    testCase "normal case" <| fun _ ->
      let editor = {
        Buffer = ["the quick brown fox"; "jumped over the lazy dog"]
        Cursor = { Row = 1; Col = 7; LastAttemptedCol = 7 }
      }
      let { Cursor = cursor } = up editor

      Expect.equal cursor.Row 0 "Should go up one row"
      Expect.equal cursor.Col 7 "Should stay in current col"
      Expect.equal cursor.LastAttemptedCol 7 "Should store last attempted row"

    testCase "staying in max col possible" <| fun _ ->
      let editor = {
        Buffer = ["the quick"; "brown fox jumped over"]
        Cursor = { Row = 1; Col = 15; LastAttemptedCol = 0 }
      }
      let { Cursor = cursor } = up editor

      Expect.equal cursor.Row 0 "Should go up one row"
      Expect.equal cursor.Col 8 "Should stay in max col possible"
      Expect.equal cursor.LastAttemptedCol 15 "Should store last attempted row"

    testCase "staying in max col possible and going back" <| fun _ ->
      let editor = {
        Buffer = ["the quick"; "brown"; "fox jumped over the lazy dog"]
        Cursor = { Row = 2; Col = 7; LastAttemptedCol = 0 }
      }
      let { Cursor = cursor } = editor |> up |> up

      Expect.equal cursor.Row 0 "Should go to top row"
      Expect.equal cursor.Col 7 "Should stay in max col possible"
      Expect.equal cursor.LastAttemptedCol 7 "Should store last attempted row"
  ]
