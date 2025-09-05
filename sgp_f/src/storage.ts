class Storage {
    static async StoreUserData(data: any) {
        try {
            localStorage.setItem('first_name', data.first_name)
            localStorage.setItem('username', data.username)
            localStorage.setItem('access', data.token)
        } catch (error) {
            console.log(error)
        }
    }    

    static RetrieveUserData() {
        try {
            const first_name = localStorage.getItem('first_name')
            const username = localStorage.getItem('username')
            const token = localStorage.getItem('access')
            return { first_name, username, token }
        } catch (error) {
            console.log(error)
        }
    }

    static async DeleteUserToken() {
        try {
            localStorage.removeItem('first_name')
            localStorage.removeItem('username')
            localStorage.removeItem('access')
        } catch (error) {
            console.log(error)
        }
    }
}

export default Storage