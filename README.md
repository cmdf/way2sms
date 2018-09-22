Send SMS through [Way2SMS] in Windows Console.
> 1. Download [exe file](https://github.com/winp/extra-bel/releases/download/1.0.0/ebel.cmd).
> 2. Copy to `C:\Program_Files\Scripts`.
> 3. Add `C:\Program_Files\Scripts` to `PATH` environment variable.
> 4. Register an account in [Way2SMS].
> 5. Send SMS from *argument*, *file (redirect)*, or *command output (pipe)*.


```batch
> oway2sms <command> [<flags>]

:: commands:
:: - get [-u|-p] [-0]
:: - set [-u <username>|-p <password>] [-i <input>] [-0]
:: - send [-u <username>] [-p <password>] [-t <to>] [-i <message (input)>] [-0]
::
:: flags:
:: -u|--username [<username>]
:: -p|--password [<password>]
:: -i|--input <input>
:: -0
```

### get

**Get** configuration values  of `username` and/or `password`. If a configuration has
been set, all commands post it will use it unless it is already they are already
specified as environment variables, or as arguments.

```batch
:: get all configuration values
> oway2sms get

:: get username configuration value
> oway2sms get -u

:: get configuration to file
> oway2sms get > <config file>
> oway2sms get > config.ini

:: get configuration with mr. interactive
> oway2sms get -0
GET CONFIGURATION
Username: 9876543210
Password: incorrect
```


### set

**Set** configuration values of `username` and/or `password`. Once configuration has
been set, all commands post it will use it unless it is already they are already
specified as environment variables, or as arguments.

```batch
:: set all configuration values
> oway2sms set -u <username> -p <password>
> oway2sms set -u 9876543210 -p incorrect

:: set password configuration value
> oway2sms set -p <password>
> oway2sms set -p incorrect

:: set configuration from file
> echo username=<username>> <config file>
> echo username=9876543210> config.ini
> echo password=<password>>> <config file>
> echo password=incorrect>> config.ini
> oway2sms set < <config file>
> oway2sms set < config.ini

:: set configuration with mr. interactive
> oway2sms set -0
SET CONFIGURATION
Username: <username>
Username: 9876543210
Password: <password>
Password: incorrect
```


### send

**Login** to Way2SMS using given `username` and `password`, and **send** SMS message
`to` specified mobile number, as specified in `input`. `username` and `password`
are special options and can be specified as *arguments*, *environment variables*, or
*configuration* set using `set` command.

```batch
:: send sms using arguments
> oway2sms send -u <username> -p <password> -t <to> -i <message(input)>
> oway2sms send -u 9876543210 -p incorrect -t 8765432109 -i "Short Message Service!"

:: send sms using environment variables
> set OWAY2SMS_USERNAME=<username>
> set OWAY2SMS_USERNAME=9876543210
> set OWAY2SMS_PASSWORD=<password>
> set OWAY2SMS_PASSWORD=incorrect
> oway2sms send -t <to> -i <message (input)>
> oway2sms send -t 8765432109 -i "Short Message Service!"

:: send sms using configuration
> oway2sms set -u <username> -p <password>
> oway2sms set -u 9876543210 -p incorrect
> oway2sms send -t <to> -i <message (input)>
> oway2sms send -t 8765432109 -i "Short Message Service!"

:: send sms with input from file text
> ping <host> > <log file>
> ping www.google.co.in > ping-www.google.co.in.log
> oway2sms send -u <username> -p <password> -t <to> < <file>
> oway2sms send -u 9876543210 -p incorrect -t 8765432109 < ping-www.google.co.in.log

:: send sms with input from command output
> ping <host> | oway2sms send -u <username> -p <password> -t <to>
> ping www.google.co.in | oway2sms send -u 9876543210 -p incorrect -t 8765432109

:: send sms with mr. interactive
> oway2sms send -0
SEND SMS
Username: <username>
Username: 9876543210
Password: <password>
Password: incorrect
To: <to>
To: 8765432109
Message:
<message (input)>
Message:
See no evil.
Hear no evil.
Speak no evil.
^Z
```




## reference

- [Way2SMS.com, a Way2Online venture][Way2SMS]
- [Kingster's Way2SMS unofficial PHP API][Way2SMS-API]

[Way2SMS]: http://site24.way2sms.com/content/index.html
[Way2SMS-API]: https://github.com/kingster/Way2SMS-API
