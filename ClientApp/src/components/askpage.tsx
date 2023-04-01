import React from 'react'
import {Page} from './page'
import {useForm, useFormState} from 'react-hook-form'

type form ={askform:string}
export const AskPage =  ()=>{
const {register,watch,handleSubmit,formState:{errors,isSubmitting,isSubmitSuccessful}}=useForm <form>({mode:"onBlur"})
const submitted=(data:form)=>{
 console.log(data)
}
watch()
    return (
        <div>
          <form onSubmit={handleSubmit(submitted)} >
          <input
           disabled={isSubmitting || isSubmitSuccessful}
          {...register("askform",{required:true,minLength:8})}

          type="text"/>
          <input type="submit"/>
          {errors.askform?.type==="minLength" ? 
          <label htmlFor="askform" style={{"background":"red","fontWeight":"bolder"}}>
            please 8 chars at least required
          </label>:""}
          {errors.askform?.type==="required" ? 
          <label htmlFor="askform" style={{"background":"red","fontWeight":"bolder"}}>
           come back and fill this piece of shit please 
          </label>:""}
          {isSubmitSuccessful && (<p style={{"background":"green"}} > good for us </p>)}
          </form>

          <Page> this is Ask Page</Page>
        </div>
    )
}

export default AskPage;
