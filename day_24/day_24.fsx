#load "../utils.fsx"

open utils

open Microsoft.FSharp.Core

type Block =
    {
        zDiv: int
        xAdd: int
        yAdd: int
        yAdd2: int
    }
    member this.calculate (w0: int) (z0: int) =
        let w = w0
        let x = z0 % 26
        let z = z0 / this.zDiv
        let x = x + this.xAdd
        let x = if x = w0 then 0 else 1
        let y = this.yAdd
        let y = x * y
        let y = y + 1
        let z = z * y
        let y = w + this.yAdd2
        let y = y * x
        let z = z + y
        z

    override this.ToString() =
        $"Block({this.zDiv},{this.xAdd},{this.yAdd},{this.yAdd2})"

let find (max: bool) (blocks: Block list) =
    let range = if max then { 9 .. -1 .. 1 } else { 1 .. 9 }
    
    let mutable seen = Set.empty<int * int>
    let mutable it = 0

    let rec innerFind (idx: int) (z: int) =
        it <- it + 1

        if idx = 14 && z = 0 then
            Some ""
        else if idx = 14 then
            None
        else if seen.Contains(idx, z) then
            None
        else
            seen <- seen.Add(idx, z)

            let mutable i = 0
            let mutable x = None

            while i < (Seq.length range) && x.IsNone do
                x <- innerFind (idx + 1) (blocks.[idx].calculate (Seq.item i range) z)
                i <- i + 1

            match x with
            | Some value -> Some(string (Seq.item (i - 1) range) + value)
            | None -> None

    innerFind 0 0


let parseBlock (lines: string list) =
    let zDiv = int lines.[4].[6..]
    let xAdd = int lines.[5].[6..]
    let yAdd = int lines.[9].[6..]
    let yAdd2 = int lines.[15].[6..]

    {
        zDiv = zDiv
        xAdd = xAdd
        yAdd = yAdd
        yAdd2 = yAdd2
    }

let parseBlocks (lines: string list) =
    lines
    |> List.chunkBySize 18
    |> List.map parseBlock

let solve () =
    file "input.txt"
    |> toRows
    |> parseBlocks
    |> tap (fun list -> list |> List.iter (fun item -> printfn "%O" item))
    |> find false
    |> tap (printf "%A")

solve ()