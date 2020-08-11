# External Use of API
To use the traqa api a user must first authenticate using an id_token this token will be required to validate requests. This token expires after sixty minutes. 


## 1.) Sign up for an account 

Navigate to https://app.traqa.io, click sign up under neath the login boxes. 
(do not sign up with a social account unless you intend to authenticate with Oauth2 flow using the auth.traqa.io endpoint see end of this post)
<kbd>
<img src="https://imgur.com/L5bKt7L.jpg" alt="1" width="500"/>
</kbd>


## 2.) Wait for Traqa to authorise your account with the correct permissions.

Please send an email to help@traqa.io with the subject as 'Requesting Access'


## 3.) Query the /getToken endpoint for an id_token

Use basic-auth to pass in the username and password you previously signed up with.

https://api.traqa.io/v1/getToken

__Postman Example:__
<kbd>
<img src="https://imgur.com/OpBDp9k.jpg" alt="2" width="1000"/>
</kbd>


__Python Example:__
```
import requests
requests.post(https://api.traqa.io/v1/getToken,auth=(username, password))
```

## 3.1) Once verfied once it is possible to retrieve a id_token by just supplying the refresh_token

A refresh token will expire after one month, to use the refresh token simply send a POST to the https://api.traqa.io/v1/getToken endpoint with the header 
'refresh_token'. 

__Postman Example:__
<kbd>
<img src="https://imgur.com/KHJCb3g.jpg" alt="2" width="850"/>
</kbd>


## 4.) Retrieve data from traqa!

You can now use any other Traqa endpoint with a header called ‘Authorization’ were value is the id_token this will validate your requests.

<kbd>
<img src="https://imgur.com/05wBo7j.jpg" alt="3" width="1000"/>
</kbd>

___

### Alternative option is implement:
Oauth2 flow can also be used to authenticate through a third party eg. google:

- callback_url: https://app.traqa.io/authenticate
- auth_url: https://auth.traqa.io/login
- grant_type: Implicit
- scope: openid
- client_id: 6mu30su2h2r9fjj6d4lvqvlc0q
