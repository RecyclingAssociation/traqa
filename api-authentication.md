# External Use of API
To use the traqa api a user must first authenticate using an id_token this token will be required to validate requests. This token expires after sixty minutes. 

1.) Sign up for an account using https://app.traqa.io, click sign up under neath the login boxes. 
(do not sign up with a social account unless you intend to authenticate with Oauth2 flow using the auth.traqa.io endpoint see end of this post)

![1](https://imgur.com/L5bKt7L.jpg)

2.) Wait for Traqa to authorise your account with the correct permissions.

3.) Query the /getToken endpoint for an id_token, using basic-auth passing in the username and password you signed up with on the front end.

https://api.traqa.io/v1/getToken

In postman this looks like so:
![2](https://imgur.com/OpBDp9k.jpg)

In python it looks like so:

```
import requests
requests.post(https://api.traqa.io/v1/getToken,auth=(username, password))
```

4.) You can now use any other Traqa endpoint with a header called ‘Authorization’ were value is the id_token this will validate your requests.
![3](https://imgur.com/05wBo7j.jpg)

### Alternative option is implement:
Oauth2 flow can also be used to authenticate through a third party eg. google:

- callback_url: https://app.traqa.io/authenticate
- auth_url: https://auth.traqa.io/login
- grant_type: Implicit
- scope: openid
- client_id: 6mu30su2h2r9fjj6d4lvqvlc0q
