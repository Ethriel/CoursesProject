export const userPostFetch = user => {
    return dispatch => {
      return fetch("https://localhost:44382/account/signin", {
        method: "POST",
        headers: {
          'Content-Type': 'application/json',
          Accept: 'application/json',
        },
        body: JSON.stringify({user})
      })
        .then(resp => resp.json())
        .then(data => {
            localStorage.setItem("token", data.token)
            dispatch(loginUser(data.user))
        })
    }
  }
  
  const loginUser = userObj => ({
      type: 'LOGIN_USER',
      payload: userObj
  })