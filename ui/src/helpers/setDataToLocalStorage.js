function setDataToLocalStorage(id, token, role, userPicture) {
    localStorage.setItem("bearer_header", `Bearer ${token}`);
    localStorage.setItem("access_token", token);
    localStorage.setItem("current_user_role", role);
    localStorage.setItem("current_user_id", id);
    localStorage.setItem("user_picture", userPicture);
}

export default setDataToLocalStorage;