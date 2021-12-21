#load "../utils.fsx"
open utils
#load "day_14_1.fsx"

open part1
open System

let increase (key1: 'a) amount (map: Map<'a, uint64>) =
    map
    |> Map.change
        key1
        (fun key ->
            match key with
            | Some value -> Some(value + amount)
            | None -> Some(amount))

let step (values: Map<string, uint64>) (pairs: Map<string, string>) =
    values
    |> Map.fold
        (fun all key value ->
            all
            |> increase (key.[..0] + pairs.[key]) value
            |> increase (pairs.[key] + key.[1..]) value)
        Map.empty

let applySteps (values: Map<string, uint64>) (steps: int) (pairs: Map<string, string>) =
    [| 0 .. steps - 1 |]
    |> Array.fold (fun polymer _ -> step polymer pairs) values

let createLetterMap (input: string) (values: Map<string, uint64>) =
    values
    |> Map.fold
        (fun letterMap pair value -> letterMap |> increase pair.[1..] value)
        ([ (input.[..0], 1UL) ] |> Map.ofSeq)

let findMinAndMax (values: Map<string, uint64>) =
    let counted = values |> Map.toArray
    (Array.minBy snd counted |> snd, Array.maxBy snd counted |> snd)

let initialMap (input: string) =
    input.ToCharArray()
    |> Array.windowed 2
    |> Array.map String
    |> Array.map (fun pair -> (pair, 1UL))
    |> Map.ofArray

let solve () =
    let inputRows = file "input.txt" |> toRows
    let valueMap = initialMap inputRows.[0]

    inputRows
    |> toPairs
    |> applySteps valueMap 40
    |> createLetterMap inputRows.[0]
    |> findMinAndMax
    |> (fun (min, max) -> max - min)
    |> printfn "%A"

solve
