module Modkit.Roles.Modules.Crypto

open System
open System.IO
open System.Security.Cryptography
open System.Text

let generateRandomString () =
    RandomNumberGenerator.GetInt32(Int32.MaxValue).ToString()
    
let hash (key: string) (data: string) =
    use stream = new MemoryStream(Encoding.UTF8.GetBytes data)
    use hmac = new HMACSHA256(Encoding.UTF8.GetBytes key)
    Convert.ToHexString(hmac.ComputeHash stream)
    