module EditorPT

  type Cursor = { Row: int; Col: int; Offset: int } 
  
  type PieceTable = {
      Original: string
      Add: string
      Pieces: Piece list
  }
  and Piece = {
      Start: int
      Length: int
      Source: Source
  }
  and Source = Original | Add
  
  type Editor = {
      Cursor: Cursor
      PieceTable: PieceTable
  }
  
  let private leftC cursor =
    { cursor with Col = cursor.Col - 1; Offset = cursor.Offset - 1 }
          
  let private rightC cursor =
    { cursor with Col = cursor.Col + 1; Offset = cursor.Offset + 1 }
      
  let left editor = 
    { editor with Cursor = leftC editor.Cursor }
  
  let right editor = 
    { editor with Cursor = rightC editor.Cursor }
  
  let insertChar { PieceTable = pieceTable; Cursor = cursor } char =
    let (bufferOffset, optionCurrentPiece) = 
      pieceTable.Pieces |>
      List.fold (fun acc elem -> 
                   match acc with
                   | (offset, Some a) -> (offset, Some a)
                   | (offset, None) -> 
                      let newOffset = offset + elem.Length
                      
                      if newOffset >= cursor.Offset then (offset, Some elem)
                      else (newOffset, None)) (0, Option<Piece>.None) 
    
    let pieces = 
        match optionCurrentPiece with 
            | None -> pieceTable.Pieces
            | Some currentPiece -> 
            pieceTable.Pieces |>
            List.fold (fun acc elem -> 
                acc @
                match elem with
                | piece when piece = currentPiece && piece.Source = Add && bufferOffset = cursor.Offset -> 
                    [
                        { Start = piece.Start; Length = piece.Length + 1; Source = Add }
                    ]
                | piece when piece = currentPiece -> 
                        [
                            { 
                                Start = currentPiece.Start
                                Length = cursor.Offset - bufferOffset
                                Source = currentPiece.Source }
                            { Start = pieceTable.Add.Length; Length = 1; Source = Add }
                            { 
                                Start = currentPiece.Start + (cursor.Offset - bufferOffset)
                                Length = currentPiece.Length - (cursor.Offset - bufferOffset) 
                                Source = currentPiece.Source }
                        ] |>
                        List.filter (fun piece -> piece.Length > 0)
                | piece -> [piece]) []
    
    let newPieceTable = { pieceTable with Pieces = pieces; Add = pieceTable.Add + char }
    { PieceTable = newPieceTable; Cursor = rightC cursor }


