#load "day_10_1.fsx"

open part1

let toPoints char =
    match char with
    | ')' -> 1
    | ']' -> 2
    | '}' -> 3
    | '>' -> 4
    | _ -> failwith "The given char was not valid"

let rec openBrackets (string: string) (stack: List<char>) =
    if string.Length = 0 then
        stack
    else
        match string.[0] with
        | c when isOpeningBracket c -> openBrackets <| string.Substring(1) <| c :: stack
        | c when isCLosingBracket c && counterPart c = stack.[0] -> openBrackets <| string.Substring(1) <| stack.Tail
        | _ -> stack

let isIncomplete line = firstError line [] = None

let toOpenBrackets lines line = openBrackets line [] :: lines

let toCounterparts x = List.map counterPart x |> List.rev

let toScores brackets =
    List.map toPoints brackets
    |> List.rev
    |> List.fold (fun score current -> score * 5UL + uint64 current) 0UL

let takeMiddle (scores: List<'a>) = scores.[scores.Length / 2]

let solve =
    file "chunks.txt"
    |> toLines
    |> List.filter isIncomplete
    |> List.fold toOpenBrackets []
    |> List.map toCounterparts
    |> List.map toScores
    |> List.sort
    |> takeMiddle

printfn $"%A{solve}"
