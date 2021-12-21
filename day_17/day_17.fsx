#load "../utils.fsx"
open utils

type Point = int * int
type Speed = Point

let targetUpperLeft = (138, -71)
let targetLowerRight = (184, -125)

let rec isHit (speed: Speed) (position: Point) (target: Point * Point) =
    let speedX, speedY = speed
    let x, y = position
    let (tx1, ty1), (tx2, ty2) = target

    // printfn "%A %A %A" speed position target

    if x > tx2 || y < ty2 then
        false
    else if x >= tx1 && x <= tx2 && y <= ty1 && y >= ty2 then
        true
    else
        isHit ((max (speedX - 1) 0), speedY - 1) (x + speedX, y + speedY) target


let simulate (min: Point) (max: Point) =
    let xMax, yMax = max

    { 1 .. xMax }
    |> Seq.fold
        (fun allHits x ->
            ({ -yMax .. -1 .. yMax }
             |> Seq.fold
                 (fun hits y ->
                     if isHit (x, y) (0, 0) (min, max) then
                         (x, y) :: hits
                     else
                         hits)
                 [])
            @ allHits)
        []

let bestHit (speed: Speed list) = speed |> List.sortByDescending snd |> List.head

let gaussianSum x = (x * (x + 1)) / 2

let solve =
    simulate targetUpperLeft targetLowerRight
    |> tap (fun x -> printfn "%A" x.Length) // Part 2
    |> bestHit
    |> tap (fun x -> printfn "%A" x)
    |> snd
    |> gaussianSum
    |> tap (fun x -> printfn "%A" x) // Part 1

solve
