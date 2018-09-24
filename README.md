# Password Checker

Stupid little .NET Core console app that checks your passwords against Troy Hunt's "Have I Been Pwned?" [Password API](https://haveibeenpwned.com/API/v2#PwnedPasswords).

Your password is hashed with the SHA-1 algorithm before it leaves your computer. It performs a lookup against Troy's API using the [k-anonymity model](https://en.wikipedia.org/wiki/K-anonymity):

1. It sends just the first 5 characters of the hash.
2. The API returns all hashes that start with those 5 characters and their frequencies.
3. The app finds the matching row and returns the corresponding frequency to the console.

This way, you never have to send your password-- nor even its entire hash-- across the internet.

## To use

1. Install [.NET Core Runtime or SDK](https://www.microsoft.com/net/download).
2. `git clone git@github.com:jdmallen/password-checker.git`
3. `cd password-checker/PasswordChecker && dotnet run`

