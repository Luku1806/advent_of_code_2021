#load "day_11_1.fsx"

open part1

let hasOnlyZeros (map: Matrix) =
    map
    |> Array.forall (Array.forall (fun value -> value = 0))

let stepsUntilAllZero (map: Matrix) =
    let mutable steps = 0

    while map |> hasOnlyZeros |> not do
        steps <- steps + 1
        step map |> ignore

    steps

let solve =
    file "map.txt"
    |> toRows
    |> toOctopusses
    |> stepsUntilAllZero
    |> printfn "%A"

solve
