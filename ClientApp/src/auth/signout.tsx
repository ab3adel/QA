import React from 'react'

import {Page} from '../components/page'
import {useAuth0} from '@auth0/auth0-react'
import {useDispatch} from 'react-redux'
import {eraseUser, eraseUserAction} from '../auth/authreducer'
type singout= "signout" | "signout-callback"
interface Props {
    action:singout
}
export const SignOut=({action}:Props)=>{
const {logout} =useAuth0()
const dispacth=useDispatch()
const signOut=async()=>{
 dispacth(eraseUserAction());
logout()
}
    return (
        <Page title="sign out">
             <button onClick={()=>signOut()}>singout</button>
        </Page>
    )
}