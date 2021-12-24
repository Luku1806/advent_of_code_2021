module utils

let file filename = System.IO.File.ReadAllText filename

let toRows (string: string) = string.Split "\n" |> Array.toList

let intersect (s1: 'a seq) (s2: 'a seq) =
    System.Linq.Enumerable.Intersect(s1, s2)
    |> List.ofSeq

let flatten (array: 'a array array) = array |> Array.reduce Array.append

let tap (action: 'T -> 'a) (value: 'T) : 'T =
    ignore (action value)
    value
