#load "../utils.fsx"

open utils

open System

type SnailfishNumber =
    | Pair of (SnailfishNumber * SnailfishNumber)
    | Value of int

type ExplodeState =
    | NotFound
    | Found of int * int
    | LeftProcessed of int
    | RightProcessed of int
    | Completed

let toInt = string >> int

let parse (line: string) =
    let tokens = line |> List.ofSeq

    let rec getElements (sfNumbers: SnailfishNumber list) tokens =
        match tokens, sfNumbers with
        | [], _ -> sfNumbers |> List.head
        | '[' :: nextTokens, _ -> nextTokens |> getElements sfNumbers
        | x :: nextTokens, _ when Char.IsDigit x ->
            nextTokens
            |> getElements (Value(x |> toInt) :: sfNumbers)
        | ',' :: nextTokens, _ -> nextTokens |> getElements sfNumbers
        | ']' :: nextTokens, r :: l :: nextFsNumbers ->
            nextTokens
            |> getElements (Pair(l, r) :: nextFsNumbers)
        | _ -> failwith "Invalid input"

    tokens |> getElements []

let explodeFirst sfNumber =
    let rec addLeftSide valueToAdd sfNumber =
        match sfNumber with
        | Value x -> Value(x + valueToAdd)
        | Pair (left, right) -> Pair(left |> addLeftSide valueToAdd, right)

    let rec addRightSide valueToAdd sfNumber =
        match sfNumber with
        | Value x -> Value(x + valueToAdd)
        | Pair (left, right) -> Pair(left, right |> addRightSide valueToAdd)

    let rec explode depth (sfNumber: SnailfishNumber) =
        match sfNumber with
        | Value _ -> NotFound, sfNumber
        | Pair (Value valueLeft, Value valueRight) when depth >= 4 -> Found(valueLeft, valueRight), sfNumber
        | Pair (left, right) ->
            let stateLeft, newLeft = left |> explode (depth + 1)

            match stateLeft with
            | Completed -> Completed, Pair(newLeft, right)
            | Found (valueLeft, valueRight) ->
                RightProcessed(valueLeft), Pair(Value(0), right |> addLeftSide valueRight)
            | RightProcessed valueLeft ->
                if depth = 0 then
                    Completed, Pair(newLeft, right)
                else
                    RightProcessed(valueLeft), Pair(newLeft, right)
            | LeftProcessed valueRight -> Completed, Pair(newLeft, right |> addLeftSide valueRight)
            | NotFound ->
                let stateRight, newRight = right |> explode (depth + 1)

                match stateRight with
                | Completed -> Completed, Pair(left, newRight)
                | Found (valueLeft, valueRight) ->
                    LeftProcessed(valueRight), Pair(left |> addRightSide valueLeft, Value(0))
                | LeftProcessed valueRight ->
                    if depth = 0 then
                        Completed, Pair(left, newRight)
                    else
                        LeftProcessed(valueRight), Pair(left, newRight)
                | RightProcessed valueLeft -> Completed, Pair(left |> addRightSide valueLeft, newRight)
                | NotFound -> NotFound, sfNumber

    explode 0 sfNumber

let rec linearize =
    function
    | Pair (left, right) -> (left |> linearize) @ (right |> linearize)
    | Value x -> [ Value(x) ]

let rec splitFirst (sfNumber: SnailfishNumber) =
    let splitPair x =
        let l = Value(Math.Floor(float (x) / 2.0) |> int)
        let r = Value(Math.Ceiling(float (x) / 2.0) |> int)
        l, r

    let rec split (hasSplit: bool) sfNumber =
        match sfNumber with
        | Pair (left, right) ->
            let leftHasSplit, left = left |> split hasSplit
            let rightHasSplit, right = right |> split leftHasSplit
            rightHasSplit, Pair(left, right)
        | Value (x) when x > 9 && (not hasSplit) -> true, Pair(splitPair x)
        | Value (x) -> hasSplit, Value(x)

    sfNumber |> (split false)

let rec calcMagnitude sfNumber =
    match sfNumber with
    | Value x -> x
    | Pair (left, right) -> 3 * calcMagnitude left + 2 * calcMagnitude right

let rec reduceNode sfNumber =
    let rec reduceExplode sfNumber =
        let state, newSfNumber = sfNumber |> explodeFirst

        match state with
        | NotFound -> newSfNumber
        | _ -> reduceExplode newSfNumber

    let hasSplit, newSfNumber = sfNumber |> reduceExplode |> splitFirst

    if hasSplit then
        reduceNode newSfNumber
    else
        newSfNumber

let rec doHomework sfNumbers =
    let folder state sfNumber =
        match state with
        | None -> Some(sfNumber |> reduceNode)
        | Some newSfNumber -> Some(Pair(newSfNumber, sfNumber) |> reduceNode)

    sfNumbers |> List.fold folder None

let rec combineAllElements list =
    match list with
    | [] -> []
    | h :: t ->
        let newList =
            seq {
                for e in t do
                    yield (h, e)
                    yield (e, h)
            }
            |> Seq.toList

        let list'' = t |> combineAllElements
        newList @ list''

let solve1 input =
    input
    |> List.map parse
    |> doHomework
    |> Option.map calcMagnitude
    |> printfn "Part 1: %A"

let solve2 input =
    input
    |> List.map parse
    |> combineAllElements
    |> List.map (fun (a, b) -> Pair(a, b) |> reduceNode |> calcMagnitude)
    |> List.max
    |> printfn "Part 2: %A"



file "input.txt" |> toRows |> solve1
file "input.txt" |> toRows |> solve2
