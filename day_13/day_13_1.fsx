module part1

#load "../utils.fsx"
open utils

open System.Text.RegularExpressions

type Dot = int * int
type Instruction = string * int

let toDots (lines: string list) : Dot List =
    lines
    |> List.filter (fun line -> line.Contains ",")
    |> List.map (fun line -> line.Split ",")
    |> List.map (fun parts -> (parts.[0] |> int, parts.[1] |> int))

let toInstructions (lines: string list) : Instruction list =
    lines
    |> List.filter (fun line -> line.Contains "fold")
    |> List.map (Regex("(?<axis>\w)=(?<value>\d+)").Match)
    |> List.map (fun parts -> (parts.Groups.["axis"].Value, parts.Groups.["value"].Value |> int))

let foldedValue current folded =
    if current > folded then
        (current - 2 * (current - folded))
    else
        current

let toFolded (dot: Dot) (instruction: Instruction) =
    let oldX, oldY = dot

    match instruction with
    | "y", pos -> (oldX, foldedValue oldY pos)
    | "x", pos -> (foldedValue oldX pos, oldY)
    | _ -> failwith "Invalid instruction"

let fold (instruction: Instruction) (dots: Dot list) =
    dots
    |> List.map (fun dot -> toFolded dot instruction)
    |> List.distinct

let solve =
    let rows = file "input.txt" |> toRows
    let dots = rows |> toDots
    let instructions = rows |> toInstructions

    dots
    |> fold instructions.[0]
    |> List.length
    |> printfn "%A"
