module part1

let file filename = System.IO.File.ReadAllText filename
let toLines (string: string) = string.Split "\n" |> Array.toList

let counterPart char =
    match char with
    | '(' -> ')'
    | ')' -> '('
    | '[' -> ']'
    | ']' -> '['
    | '{' -> '}'
    | '}' -> '{'
    | '<' -> '>'
    | '>' -> '<'
    | _ -> failwith "The given char was not valid"

let isOpeningBracket char =
    match char with
    | '('
    | '['
    | '{'
    | '<' -> true
    | _ -> false

let isCLosingBracket char =
    match char with
    | ')'
    | ']'
    | '}'
    | '>' -> true
    | _ -> false

let toErrorPoints char =
    match char with
    | ')' -> 3
    | ']' -> 57
    | '}' -> 1197
    | '>' -> 25137
    | _ -> failwith "The given char was not valid"

let rec firstError (string: string) (stack: List<char>) =
    if string.Length = 0 then
        None
    else
        match string.[0] with
        | c when isOpeningBracket c -> firstError <| string.Substring(1) <| c :: stack
        | c when isCLosingBracket c && List.isEmpty stack -> Some c
        | c when isCLosingBracket c && counterPart c = stack.[0] -> firstError <| string.Substring(1) <| stack.Tail
        | c -> Some c

let addIfHasError (allErrors: List<char>) line =
    match firstError line [] with
    | Some error -> error :: allErrors
    | None -> allErrors

let solve =
    file "chunks.txt"
    |> toLines
    |> List.fold addIfHasError []
    |> List.map toErrorPoints
    |> List.sum

printfn $"%d{solve}"
