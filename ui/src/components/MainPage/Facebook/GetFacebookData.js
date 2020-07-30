const GetFacebookData = response => {
    const userData = {
        firstName: response.first_name,
        lastName: response.last_name,
        email: response.email,
        accessToken: response.accessToken,
        pictureUrl: response.picture.data.url,
        userId: response.userID
    };

    return userData;
}

export default GetFacebookData;