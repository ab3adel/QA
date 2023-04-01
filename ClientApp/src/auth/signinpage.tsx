import React from 'react'

import {useAuth0} from '@auth0/auth0-react'
type singin= 'signin' | 'signin-callback'
interface Props {
    action:singin
}
export const SignInPage =({action}:Props) =>{
   const {loginWithRedirect,isLoading} =useAuth0()
  if (isLoading) {
      return (
          <div>
              wait a moment
          </div>
      )
  }
    return (
        <div>
            <button onClick={()=>loginWithRedirect()}>login</button>
        </div>
    )
}