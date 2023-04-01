import React from 'react'
import {AnswerData} from './questiondata'
import {Answer} from './answer'
interface Props {data:AnswerData []}

export const AnswerList =({data}:Props)=>{

    return (
        <ul>
       { data.map((ele,index)=>{
           return(
         <li key={index}>
            <Answer data={ele}/>
         </li>
           )
       })}
        </ul>
    )
}