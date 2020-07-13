function setDataToLocalStorage(id, token, role) {
    localStorage.setItem("bearer_header", `Bearer ${token}`);
    localStorage.setItem("access_token", token);
    localStorage.setItem("current_user_role", role);
    localStorage.setItem("current_user_id", id);
}

export default setDataToLocalStorage;