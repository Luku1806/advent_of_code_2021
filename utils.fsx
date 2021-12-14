module utils
let file filename = System.IO.File.ReadAllText filename

let toRows (string: string) = string.Split "\n" |> Array.toList

