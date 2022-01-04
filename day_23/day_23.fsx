#load "../utils.fsx"

open Microsoft.FSharp.Collections
open utils

open Microsoft.FSharp.Core

type Amphipod =
    | Amber
    | Bronce
    | Copper
    | Desert
    member this.EnergyConsumption =
        match this with
        | Amber -> 1
        | Bronce -> 10
        | Copper -> 100
        | Desert -> 1000

    member this.Position =
        match this with
        | Amber -> 2
        | Bronce -> 4
        | Copper -> 6
        | Desert -> 8

type Field =
    | Amphipod of Amphipod
    | Floor

type State =
    {
        roomA: Field array
        roomB: Field array
        roomC: Field array
        roomD: Field array
        hallway: Field array
        energySpent: int
    }
    member this.IsFinished =
        this.roomA
        |> Array.take 2
        |> Array.forall (fun field -> field = Amphipod Amber)
        && this.roomB
           |> Array.take 2
           |> Array.forall (fun field -> field = Amphipod Bronce)
        && this.roomC
           |> Array.take 2
           |> Array.forall (fun field -> field = Amphipod Copper)
        && this.roomD
           |> Array.take 2
           |> Array.forall (fun field -> field = Amphipod Desert)

    member this.RoomFor(amphipod: Amphipod) =
        match amphipod with
        | Amber -> this.roomA
        | Bronce -> this.roomB
        | Copper -> this.roomC
        | Desert -> this.roomD

let allowedDirections = [| 0; 1; 3; 5; 7; 9; 10 |]

let canTravel (state: State) (start: int) (stop: int) =
    let direction = if start > stop then -1 else 1

    { start .. direction .. stop }
    |> Seq.forall (fun index -> state.hallway.[index] = Floor)


let travelOut (state: State) (start: int) (amphipod: Amphipod) =
    allowedDirections
    |> Array.fold
        (fun out stop ->
            if canTravel state start stop then
                //prinfn "Travelable %d" stop
                let newHallway = Array.copy state.hallway
                newHallway.[stop] <- Field.Amphipod amphipod

                let newEnergySpent =
                    (1 + abs (stop - start))
                    * amphipod.EnergyConsumption

                { state with
                    hallway = newHallway
                    energySpent = newEnergySpent
                }
                :: out
            else
                out)
        []


let energyBoundary (state: State) =
    let inHallway =
        allowedDirections
        |> Array.fold
            (fun boundary index ->
                match state.hallway.[index] with
                | Floor -> boundary
                | Amphipod amphipod ->
                    boundary
                    + (1
                       + (abs (index - amphipod.Position))
                         * amphipod.EnergyConsumption))
            0

    [|
        Amphipod.Amber
        Amphipod.Bronce
        Amphipod.Copper
        Amphipod.Desert
    |]
    |> Array.indexed
    |> Array.fold
        (fun boundary (index, amphipod) ->
            state.RoomFor amphipod
            |> Array.fold
                (fun boundary1 current ->
                    match current with
                    | Field.Amphipod current when current <> amphipod ->
                        boundary1
                        + ((index
                            + (abs (amphipod.Position - current.Position)))
                           * current.EnergyConsumption)
                    | _ -> boundary1)
                boundary)
        inHallway

let sortRoomFor (state: State) (amphipodType: Amphipod) =
    let room = state.RoomFor amphipodType

    if room |> Array.exists (fun field -> field = Floor) then
        let pivot =
            room
            |> Array.take 2
            |> Array.rev
            |> Array.takeWhile
                (fun current ->
                    current = Field.Amphipod amphipodType
                    || current = Floor)
            |> Array.length

        let newRoom, energyDelta =
            { 0 .. (2 - pivot - 1) }
            |> Seq.fold
                (fun (room: Field array, energyDelta) i ->
                    match room.[i] with
                    | Field.Amphipod _ -> (room, energyDelta)
                    | Floor ->
                        { i + 1 .. (2 - pivot - 1) }
                        |> Seq.tryFind (fun j -> room.[j] <> Floor)
                        |> fun j ->
                            match j with
                            | None -> (room, energyDelta)
                            | Some j ->
                                match room.[j] with
                                | Floor -> (room, energyDelta)
                                | Amphipod amphipod ->
                                    let newRoom = Array.copy room
                                    let a = room.[j]
                                    room.[j] <- room.[i]
                                    room.[i] <- a
                                    (newRoom, energyDelta + (j - i) * amphipod.EnergyConsumption))
                (room, 0)

        let newRoom', energyDelta' =
            { (2 - pivot) .. 2 - 1 }
            |> Seq.fold
                (fun (newRoom': Field [], energyDelta) i ->
                    match newRoom.[i] with
                    | Floor -> (newRoom', energyDelta)
                    | _ ->
                        { i + 1 .. 1 }
                        |> Seq.tryFind (fun j -> newRoom'.[j] = Floor)
                        |> fun j ->
                            match j with
                            | None -> (newRoom', energyDelta)
                            | Some j ->
                                match newRoom'.[j] with
                                | Amphipod _ -> (newRoom', energyDelta)
                                | Floor ->
                                    let newRoom'' = Array.copy newRoom'
                                    let a = newRoom''.[j]
                                    newRoom''.[j] <- newRoom''.[i]
                                    newRoom''.[i] <- a

                                    (newRoom'',
                                     energyDelta
                                     + (j - i) * amphipodType.EnergyConsumption))
                (newRoom, energyDelta)


        let newState =
            { state with
                energySpent = state.energySpent + energyDelta'
            }

        match amphipodType with
        | Amber -> { newState with roomA = newRoom' }
        | Bronce -> { newState with roomB = newRoom' }
        | Copper -> { newState with roomC = newRoom' }
        | Desert -> { newState with roomD = newRoom' }
    else
        state

let travelIn (state: State) (start: int) (amphipod: Amphipod) =
    if state.RoomFor(amphipod).[0] = Floor
       && canTravel state start amphipod.Position then
        let newHallway = Array.copy state.hallway
        newHallway.[start] <- Floor

        let newEnergySpent =
            state.energySpent
            + (1 + (abs (amphipod.Position - start)))
              * amphipod.EnergyConsumption

        let newState =
            { state with
                energySpent = newEnergySpent
                hallway = newHallway
            }

        let newRoom = state.RoomFor amphipod
        newRoom.[0] <- Field.Amphipod amphipod

        Some(
            match amphipod with
            | Amber -> { newState with roomA = newRoom }
            | Bronce -> { newState with roomB = newRoom }
            | Copper -> { newState with roomC = newRoom }
            | Desert -> { newState with roomD = newRoom }
        )
    else
        None

let possibleMoves (state: State) =
    let sortedState =
        [|
            Amphipod.Amber
            Amphipod.Bronce
            Amphipod.Copper
            Amphipod.Desert
        |]
        |> Array.fold sortRoomFor state

    let possibleMoves =
        allowedDirections
        |> Array.fold
            (fun states direction ->
                match sortedState.hallway.[direction] with
                | Floor ->
                    //prinfn "X %d" direction
                    states
                | Amphipod amphipod ->
                    //prinfn "Y %d" direction
                    let newState = travelIn sortedState direction amphipod

                    match newState with
                    | None -> states
                    | Some newState -> newState :: states)
            List.empty<State>

    [|
        Amphipod.Amber
        Amphipod.Bronce
        Amphipod.Copper
        Amphipod.Desert
    |]
    |> Array.fold
        (fun (states: State list) amphipodType ->
            let room = state.RoomFor amphipodType

            match room.[0] with
            | Floor -> states
            | Amphipod amphipod ->
                if amphipod <> amphipodType
                   || room.[1] <> Field.Amphipod amphipodType then
                    travelOut state amphipodType.Position amphipod
                    |> List.fold
                        (fun states state ->
                            let newRoom = Array.copy (state.RoomFor amphipodType)
                            newRoom.[0] <- Floor

                            let newState =
                                match amphipod with
                                | Amber -> { state with roomA = newRoom }
                                | Bronce -> { state with roomB = newRoom }
                                | Copper -> { state with roomC = newRoom }
                                | Desert -> { state with roomD = newRoom }

                            newState :: states)
                        states
                else
                    states)
        possibleMoves

let bestSolution (state: State) =
    let rec innerBestSolution (toCheck: State list) (alreadyChecked: Set<State>) (best: int) =
        printfn "%d" toCheck.Length

        if toCheck.IsEmpty then
            printfn "Empty"
            best
        else if toCheck.Head.IsFinished then
            printfn "Finished"
            innerBestSolution toCheck.Tail alreadyChecked (min toCheck.Head.energySpent best)
        else if alreadyChecked.Contains toCheck.Head then
            printfn "Seen"
            innerBestSolution toCheck.Tail alreadyChecked best
        else if toCheck.Head.energySpent
                + energyBoundary toCheck.Head > best then
            printfn "Too large"
            innerBestSolution toCheck.Tail alreadyChecked best
        else
            printfn "Go on"
            innerBestSolution (toCheck.Tail @ possibleMoves toCheck.Head) (alreadyChecked.Add toCheck.Head) best

    innerBestSolution [ state ] Set.empty 20000


let mutableBestSolution (state: State) =
    let mutable best = 20000
    let mutable toCheck = [ state ]
    let mutable alreadySeen = Set.empty<State>

    let mutable i = 0

    while not toCheck.IsEmpty && i <= 20 do
        let current = toCheck.Head
        i <- i + 1
        toCheck <- toCheck.Tail

        if current.IsFinished then
            best <- min current.energySpent best
        else if alreadySeen.Contains current then
            ()
        else if current.energySpent + energyBoundary current > best then
            ()
        else
            let moves = possibleMoves current

            toCheck <- List.append toCheck moves
            alreadySeen <- alreadySeen.Add current

let parseInput (lines: string list) =
    let fields =
        lines
        |> List.skip 2
        |> List.filter (fun line -> not <| line.Trim().StartsWith("####"))
        |> List.map
            (fun line ->
                line.Trim().ToCharArray()
                |> Array.where (fun c -> c <> '#'))
        |> List.map
            (fun line ->
                line
                |> Array.map
                    (fun char ->
                        match char with
                        | 'A' -> Field.Amphipod Amber
                        | 'B' -> Field.Amphipod Bronce
                        | 'C' -> Field.Amphipod Copper
                        | 'D' -> Field.Amphipod Desert
                        | _ -> failwith "Invalid amphipod"))

    let filledFields =
        if fields.Length < 10 then
            fields
            @ [
                Array.init 4 (fun _ -> Floor)
                Array.init 4 (fun _ -> Floor)
            ]
        else
            fields

    {
        roomA =
            [|
                filledFields.[0].[0]
                filledFields.[1].[0]
                filledFields.[2].[0]
                filledFields.[3].[0]
            |]
        roomB =
            [|
                filledFields.[0].[1]
                filledFields.[1].[1]
                filledFields.[2].[1]
                filledFields.[3].[1]
            |]
        roomC =
            [|
                filledFields.[0].[2]
                filledFields.[1].[2]
                filledFields.[2].[2]
                filledFields.[3].[2]
            |]
        roomD =
            [|
                filledFields.[0].[3]
                filledFields.[1].[3]
                filledFields.[2].[3]
                filledFields.[3].[3]
            |]
        hallway = Array.init 11 (fun _ -> Floor)
        energySpent = 0
    }


let solve () =
    let initialState =
        {
            roomA = [| Field.Amphipod Amber; Floor; Floor; Floor |]
            roomB = [| Floor; Field.Amphipod Desert; Floor; Floor |]
            roomC = Array.init 4 (fun _ -> Floor)
            roomD = Array.init 4 (fun _ -> Floor)
            hallway = Array.init 11 (fun _ -> Floor)
            energySpent = 0
        }

    let initialState =
        {
            roomA =
                [|
                    Field.Amphipod Amber
                    Field.Amphipod Amber
                    Floor
                    Floor
                |]
            roomB =
                [|
                    Field.Amphipod Bronce
                    Field.Amphipod Bronce
                    Floor
                    Floor
                |]
            roomC =
                [|
                    Field.Amphipod Copper
                    Field.Amphipod Copper
                    Floor
                    Floor
                |]
            roomD =
                [|
                    Field.Amphipod Desert
                    Field.Amphipod Desert
                    Floor
                    Floor
                |]
            hallway = Array.init 11 (fun _ -> Floor)
            energySpent = 0
        }

    printfn "%b" initialState.IsFinished

    file "input.txt"
    |> toRows
    |> parseInput
    |> tap (printfn "%A")
    |> mutableBestSolution
    |> tap (printfn "%A")


//initialState
//|> fun state -> sortRoomFor state Bronce
//|> tap (//prinfn "%A")
//|> fun state -> sortRoomFor state Amber
//|> tap (//prinfn "%A")
//|> fun state -> sortRoomFor state Amber
//|> tap (//prinfn "%A")

solve ()
