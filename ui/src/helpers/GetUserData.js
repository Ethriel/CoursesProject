const GetUserData = (user) => {
    const userData = {
        id: user.id,
        email: user.email,
        roleName: user.roleName,
        birthDate: user.birthDate,
        age: user.age,
        registeredDate: user.registeredDate,
        avatarPath: user.avatarPath,
    };
    return userData;
};

export default GetUserData;