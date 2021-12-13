module part1

type Matrix = int array array

let file filename = System.IO.File.ReadAllText filename

let toRows (string: string) = string.Split "\n"

let inline charToInt c = int c - int '0'

let toIntegers (line: string) =
    line.ToCharArray() |> Array.map charToInt

let toOctopusses (lines: string array) = lines |> Array.map toIntegers

let chargeIfPossible (mat: Matrix) x y =
    match (x, y) with
    | x, y when
        y >= 0
        && y < mat.Length
        && x >= 0
        && x < mat.[y].Length
        && mat.[y].[x] <> 0
        ->
        mat.[y].[x] <- mat.[y].[x] + 1
    | _ -> ()

let chargeAround (mat: Matrix) x y =
    if (mat.[y].[x] > 9) then
        mat.[y].[x] <- 0

        chargeIfPossible mat (x - 1) y
        chargeIfPossible mat (x + 1) y
        chargeIfPossible mat x (y - 1)
        chargeIfPossible mat x (y + 1)
        chargeIfPossible mat (x - 1) (y - 1)
        chargeIfPossible mat (x - 1) (y + 1)
        chargeIfPossible mat (x + 1) (y - 1)
        chargeIfPossible mat (x + 1) (y + 1)

        true
    else
        false

let containsChargedUp (mat: Matrix) =
    mat
    |> Array.fold Array.append [||]
    |> Array.exists (fun value -> value > 9)

let rec chainReaction (mat: Matrix) =
    let mutable flashes = 0

    for y = 0 to mat.Length - 1 do
        for x = 0 to mat.[y].Length - 1 do
            if chargeAround mat x y then
                flashes <- flashes + 1


    if containsChargedUp mat then
        flashes + chainReaction mat
    else
        flashes

let rec step (mat: Matrix) =
    for y = 0 to mat.Length - 1 do
        for x = 0 to mat.[y].Length - 1 do
            mat.[y].[x] <- mat.[y].[x] + 1

    chainReaction mat

let steps (mat: Matrix) steps =
    [| 0 .. steps - 1 |]
    |> Array.fold (fun flashes _ -> flashes + step mat) 0


let solve =
    file "map.txt" |> toRows |> toOctopusses |> steps
    <| 100
    |> printfn "%d"

solve
