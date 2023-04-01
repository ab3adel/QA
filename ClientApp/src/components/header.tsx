import * as React from 'react';
import { Search } from './search';
import {Navbar,NavItem,Nav} from  'reactstrap'
import {SignInPage} from '../auth/signinpage'
import {useAuth0,User} from '@auth0/auth0-react'
import {SignOut} from '../auth/signout'
import {useSelector,useDispatch} from 'react-redux'

import {AuthState} from '../auth/authreducer'
 export const Header =()=>{
const IsAuthenticated = useSelector((state:AuthState)=>{
    return state.auth.isAuthenticated
})
const UserObj = useSelector((state:AuthState)=>{
return state.auth.user
})
const {isAuthenticated,user} =useAuth0()
console.log(IsAuthenticated)
    return (
<Nav>
       <Navbar>
           <NavItem>
           <h3> QA</h3>
           </NavItem>
           <NavItem>
           <Search/>
           </NavItem>
           <NavItem>
           { isAuthenticated? <React.Fragment> 
                <span> {user?.name}</span>
               <SignOut action="signout"/></React.Fragment> 
                : <SignInPage action="signin"/>}
           </NavItem>
        </Navbar>
        </Nav>
    )
}