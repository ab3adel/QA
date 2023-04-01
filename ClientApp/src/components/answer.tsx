import React from 'react'
import {AnswerData} from './questiondata'
interface Props {
    data:AnswerData
}
export const Answer =({data}:Props)=>{
    return (
        <div>
            {data.content}
            {`Answered by ${data.userName} on
        ${data.created.toLocaleDateString()} 
        ${data.created.toLocaleTimeString()}`}
        </div>

    )
}