import React from 'react'
import createAuth0Client from '@auth0/auth0-spa-js'
import Auth0Client from '@auth0/auth0-spa-js/dist/typings/Auth0Client'
import {auth0Settings} from '../components/appsettings'
import {Auth0Provider,useAuth0} from '@auth0/auth0-react'
import {useSelector,useDispatch} from 'react-redux'
import {gettingUserAction,GotUserAction} from './authreducer'
interface Auth0User {
    name:string,
    email:string
}
interface IAuth0Context {
    isAuthenticated:boolean,
    user?:Auth0User,
    
    loading:boolean
}
 


export const getAccessToken =async ()=>{
    const auth0FromHook= await createAuth0Client(auth0Settings);
    const accessToken =auth0FromHook.getTokenSilently()
    return accessToken
}

export const Auth0Context = React.createContext <IAuth0Context>({
    isAuthenticated:false,
    
    loading:true
})



export const AuthProvider :React.FC=({children})=> {
    const dispatch = useDispatch()
const [IsAuthenticated,setIsAuthenticated]=React.useState<boolean>(false)
const [User,setUser] =React.useState<Auth0User | undefined>(undefined)
const [auth0Client,setAuth0Client]=React.useState<Auth0Client>()
const [loading,setLoading]=React.useState<boolean>(true)

const {isAuthenticated,user} =useAuth0 ()
const getAuth0ClientFromState=()=>{
    if (auth0Client=== undefined){
        throw new Error('eror auth0 not set')
    }
    return auth0Client
}
React.useEffect(()=>{
if (isAuthenticated){
    dispatch (gettingUserAction());
    if (user && user?.name && user?.email){
        console.log(isAuthenticated,user)
     dispatch(GotUserAction({name:user?.name,email:user?.email}))
    
    }
}
},[])
return (
    <Auth0Provider
    domain={auth0Settings.domain}
    clientId={auth0Settings.client_id}
    redirectUri={auth0Settings.redirect_uri}>
 <Auth0Context.Provider value={
  { isAuthenticated:IsAuthenticated,
   loading:false,
   user:User}
 }>
        {children}
        </Auth0Context.Provider>
    </Auth0Provider>
)
}
