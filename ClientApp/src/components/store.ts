import {QuestionData} from './questiondata'
import {authReducer,AuthState} from '../auth/authreducer'
import {Store,createStore,combineReducers} from 'redux'
import { useReducer } from 'react'
interface QuestionState {
    readonly loading:boolean,
    readonly unanswerd:QuestionData[],
    readonly viewing :QuestionData | null,
    readonly searched:QuestionData[]
}
export interface AppState {
    questions:QuestionState
}

const initialQuestionState:QuestionState={
    loading:false,
    unanswerd:[],
    viewing:null,
    searched:[]
}
export const GETTINGUNANSWERDQUESTIONS="gettingunansweredquestions"
export const gettingUnAnsweredQuestionsAction=()=>({
    type:GETTINGUNANSWERDQUESTIONS
} as const)
 
export const GOTUNANSWEREDQUESTIONS="gotunansweredquestions"
export const gotUnAnsweredQuestionsAction=(questions:QuestionData[])=>({
type:GOTUNANSWEREDQUESTIONS,
questions
}as const)

export const GETTINGQUESTION="gittingquestion"
export const gettingQuestionAction=()=>({
    type:GETTINGQUESTION
} as const )

export const GOTQUESTION= "gotquestion"
export const gotQuestionAction=(question:QuestionData |null )=>({
type:GOTQUESTION,
question:question
}as const)

export const SEARCHINGQUESTIONS="searchedquestions"
export const searchingQuestionsAction=()=>({
    type:SEARCHINGQUESTIONS
}as const)

export const SEARCHEDQUESTIONS="searchedquestion"
export const searchedQuestionsAction=(questions:QuestionData[])=>({
type:SEARCHEDQUESTIONS,
questions
}as const)

type QuestionsActions =
  | ReturnType<typeof gettingUnAnsweredQuestionsAction>

  | ReturnType<typeof gotUnAnsweredQuestionsAction>

  | ReturnType<typeof gettingQuestionAction>

  | ReturnType<typeof gotQuestionAction>

  | ReturnType<typeof searchingQuestionsAction>

  | ReturnType<typeof searchedQuestionsAction>;

const questionsReducer=(state=initialQuestionState,action:QuestionsActions)  =>{
    switch (action.type){
        case GETTINGUNANSWERDQUESTIONS:
            return{
             ...state ,
             loading:true
        }
        case GOTUNANSWEREDQUESTIONS : 
           return {
            ...state ,
            unanswerd:action.questions,
            loading:false
        }
        case GETTINGQUESTION : 
        return {
            ...state,
            
            loading:true
        }
        case GOTQUESTION :
            return {
                ...state,
                viewing:action.question,
                loading:false
            }
        case SEARCHINGQUESTIONS :
            return {
                ...state,
               loading:true
            }  
        case SEARCHEDQUESTIONS : 
            return {
                ...state,
                searched:action.questions,
                loading:false
            }
        default :
        return state
    }
    
}
const RootReducer =combineReducers<AppState &AuthState>({
    questions:questionsReducer,
    auth : authReducer
})
export function configureStore():Store<AppState & AuthState>{
    const store=createStore(
        RootReducer,
        undefined
    )
    return store;
}

