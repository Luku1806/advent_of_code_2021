#load "../utils.fsx"

open utils

type Map = char array array

let parseMap (lines: string list) =
    lines
    |> Array.ofList
    |> Array.map (fun line -> line.ToCharArray())

let countDifferences (map1: Map) (map2: Map) =
    (map1, map2)
    ||> Array.fold2
            (fun changes line1 line2 ->
                changes
                + ((line1, line2)
                   ||> Array.fold2 (fun changes value1 value2 -> if value1 <> value2 then changes + 1 else changes) 0))
            0

let moveNorth (map: Map) : Map =
    let newMap = map |> Array.map Array.copy

    for x in 0 .. map.[0].Length - 1 do
        for y in 0 .. map.Length - 1 do
            let newX = if x >= newMap.[0].Length - 1 then 0 else x + 1

            if map.[y].[x] = '>' && map.[y].[newX] = '.' then
                newMap.[y].[x] <- '.'
                newMap.[y].[newX] <- '>'

    newMap

let moveSouth (map: Map) =
    let newMap = map |> Array.map Array.copy

    for x in 0 .. map.[0].Length - 1 do
        for y in 0 .. map.Length - 1 do
            let newY = if y >= newMap.Length - 1 then 0 else y + 1

            if map.[y].[x] = 'v' && map.[newY].[x] = '.' then
                newMap.[y].[x] <- '.'
                newMap.[newY].[x] <- 'v'

    newMap

let fullStep map = map |> moveNorth |> moveSouth

let printMap (map: Map) =
    map
    |> Array.iter (fun line -> printfn "%s" (System.String line))

    printfn ""

let findStandingMap (map: Map) =
    Seq.initInfinite id
    |> Seq.scan (fun (_, _, newMap) i -> (i + 3, newMap, newMap |> fullStep)) (0, map, fullStep map)
    |> Seq.takeWhile (fun (_, before, current) -> countDifferences before current <> 0)
    |> Seq.last

let solve () =
    file "input.txt"
    |> toRows
    |> parseMap
    |> findStandingMap
    |> tap (fun (steps, _, _) -> printfn "Final Result: %d" steps)
    |> tap (fun (_, _, finalMap) -> printMap finalMap)

solve ()
