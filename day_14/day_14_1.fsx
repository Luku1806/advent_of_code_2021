module part1

open System

#load "../utils.fsx"
open utils

let toPairs (lines: string list) =
    lines
    |> List.filter (fun line -> line.Contains "->")
    |> List.map (fun line -> line.Split "->")
    |> List.map (fun parts -> (parts.[0].[0..1].Trim(), parts.[1].[0..1].Trim()))
    |> Map.ofList

let step (input: string) (pairs: Map<string, string>) = 
    let newPolymer = input.ToCharArray()
                     |> Array.windowed 2
                     |> Array.map String
                     |> Array.map (fun pair -> pairs.[pair] + pair.[1..])
                     |> String.Concat
    input[..0] + newPolymer

let applySteps (input: string) (steps: int) (pairs: Map<string, string>) =
      [|0..steps - 1|] |> Array.fold (fun polymer _ -> step polymer pairs) input
      
let findMinAndMax (polymer: string) = 
    let counted = polymer.ToCharArray() |> Array.countBy id
    (Array.minBy snd counted |> snd, Array.maxBy snd counted |> snd)

let solve () =
    let inputRows = file "input.txt" |> toRows
    
    inputRows 
    |> toPairs
    |> applySteps inputRows.[0] 10
    |> findMinAndMax
    |> (fun (min, max) -> max - min)
    |> printf "%d"