# text-editor
console text editor written in fsharp

## walk-through

#### Program.fs

imperative shell; maintains a mutable editor and renders it appropriately with every keystroke

#### Editor.fs

where the fun is: defines an Editor type and exposes functions on it (`up`, `down`, `insertChar`, `removeChar` etc)

#### EditorPieceTable.fs

a failed attempt to use a piece table data structure to store the text instead of a string array
