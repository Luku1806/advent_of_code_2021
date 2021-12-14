#load "../utils.fsx"
open utils

#load "day_13_1.fsx"
open part1

let draw (dots: Dot list) =
    let maxX = List.map fst dots |> List.max
    let maxY = List.map snd dots |> List.max

    for y = 0 to maxY do
        for x = 0 to maxX do
            if dots |> List.contains (x, y) then
                printf "#"
            else
                printf "."

        printfn ""

let solve =
    let rows = file "input.txt" |> toRows
    let dots = rows |> toDots
    let instructions = rows |> toInstructions

    List.fold (fun finalDots x -> fold x finalDots) dots instructions
    |> draw
