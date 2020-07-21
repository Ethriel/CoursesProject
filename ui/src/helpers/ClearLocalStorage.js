const ClearLocalStorage = () => {
    localStorage.setItem("bearer_header", undefined);
    localStorage.setItem("access_token", undefined);
    localStorage.setItem("current_user_role", undefined);
    localStorage.setItem("current_user_id", undefined);
    localStorage.setItem("user_picture", undefined);
};

export default ClearLocalStorage;