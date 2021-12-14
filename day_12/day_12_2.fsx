#load "day_12_1.fsx"
open part1

#load "../utils.fsx"
open utils

let overallLimitReached (visited: string list) =
    visited |> List.distinct |> List.length < visited.Length

let rec pathsToEnd (cave: string) (visitedCaves: string list) maxSmall (connections: Connections) =
    let newCaves = connections.[cave]

    if cave = "end" then
        1
    else if cave = "start"
            && visitedCaves |> List.contains cave then
        0
    else if overallLimitReached visitedCaves
            && visitedCaves |> List.contains cave then
        0
    else if isSmallCave cave then
        newCaves
        |> List.fold
            (fun count next ->
                count
                + pathsToEnd next (cave :: visitedCaves) maxSmall connections)
            0
    else
        newCaves
        |> List.fold
            (fun count next ->
                count
                + pathsToEnd next visitedCaves maxSmall connections)
            0

let solve =
    file "input.txt"
    |> toRows
    |> List.map toConnection
    |> List.fold upsertConnection Map.empty
    |> pathsToEnd "start" [] 1
    |> printfn "%A"
