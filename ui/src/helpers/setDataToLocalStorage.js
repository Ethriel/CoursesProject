const setDataToLocalStorage = (id, token, role, userPicture, email, emailConfirmed) => {
    localStorage.setItem("bearer_header", `Bearer ${token}`);
    localStorage.setItem("access_token", token);
    localStorage.setItem("current_user_role", role);
    localStorage.setItem("current_user_id", id);
    localStorage.setItem("user_avatar", userPicture);
    localStorage.setItem("current_user_email", email);
    localStorage.setItem("current_user_email_confirmed", emailConfirmed);
}

export default setDataToLocalStorage;