module part1

#load "../utils.fsx"
open utils

type Connection = string * string
type Connections = Map<string, string list>

let toConnection (line: string) : Connection =
    let parts = line.Split '-'
    (parts.[0], parts.[1])

let upsert key value (map: Connections) =
    map
    |> Map.change
        key
        (fun key ->
            match key with
            | Some list -> Some(value :: list)
            | None -> Some([ value ]))

let upsertConnection (map: Connections) (connection: Connection) =
    let start, stop = connection
    map |> upsert start stop |> upsert stop start

let isSmallCave (string: string) = string.ToLower() = string

let canVisit (visited: string list) node =
    List.filter (fun current -> current = node) visited
    |> List.length < 1


let rec pathsToEnd (cave: string) (visitedSmallCaves: string list) (connections: Connections) =
    let newCaves = connections.[cave]

    if cave = "end" then
        1
    else if not <| canVisit visitedSmallCaves cave then
        0
    else if isSmallCave cave then
        newCaves
        |> List.fold
            (fun count next ->
                count
                + pathsToEnd next (cave :: visitedSmallCaves) connections)
            0
    else
        newCaves
        |> List.fold
            (fun count next ->
                count
                + pathsToEnd next visitedSmallCaves connections)
            0

let solve =
    file "input.txt"
    |> toRows
    |> List.map toConnection
    |> List.fold upsertConnection Map.empty
    |> pathsToEnd "start" []
    |> printfn "%A"
