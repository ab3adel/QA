import { gettingUnAnsweredQuestionsAction } from "../components/store"

export interface UserData {
    name:string,
    email:string
}
export interface UserState {
   readonly user:UserData |null ,
  readonly isAuthenticated:boolean,
  readonly isLoading : boolean,
}
export interface AuthState{
    auth:UserState
}
const initialState:UserState ={
    user:null,
    isAuthenticated:false,
    isLoading:true
}

export const  gettingUser ="GETTINGUSER"
export const gettingUserAction=()=>({
    type:gettingUser
} as const)

export const gotUser = "GOTUSER"
export const GotUserAction=(user:UserData ) =>({
    type:gotUser,
    user
})
export const eraseUser = "ERASEUSER"
export const eraseUserAction =()=>({
    type:eraseUser
} as const)
type UserActions  = 
ReturnType <typeof gettingUserAction >|
ReturnType <typeof GotUserAction> | 
ReturnType <typeof eraseUserAction>


export const authReducer =(state=initialState,action:UserActions)=>{
            switch (action.type) {
                case gettingUser:
                    return{
                         ...state,

                     }
                case gotUser :
                    return {
                      
                        user:action.user,
                        isAuthenticated:true,
                        isLoading:false
                    } 
                case eraseUser:
                    return {
                        ...state,
                        user:{
                            name:'',
                            email:''
                        },
                        isAuthenticated:false
                    }    
                    default:
                        return state    
            }
}
